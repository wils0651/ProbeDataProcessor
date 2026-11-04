#!/bin/bash
set -e

VM_USER="tim"
VM_HOST="192.168.50.199"
VM_DEPLOY_DIR="/opt/apps/ProbeDataProcessor"
PROJECT_DIR="/home/tim/Repos/ProbeDataProcessor/ProbeDataProcessor"
SERVICE_NAME="probeDataProcessor.service"

echo "Building..."
cd "$PROJECT_DIR"
dotnet publish -c Release -r linux-x64 --no-self-contained -o ./publish

echo "Ensuring ${VM_DEPLOY_DIR} exists on the VM..."
ssh -t ${VM_USER}@${VM_HOST} "test -d ${VM_DEPLOY_DIR} || (sudo mkdir -p ${VM_DEPLOY_DIR} && sudo chown ${VM_USER}: ${VM_DEPLOY_DIR})"

echo "Copying to VM..."
rsync -avz --delete ./publish/ ${VM_USER}@${VM_HOST}:${VM_DEPLOY_DIR}/

# The daily timer is what actually schedules this job; the service alone only runs at boot. Install both
# units when they differ from what's on the VM (or the timer isn't enabled). When nothing changed this
# needs no sudo, so ordinary deploys don't prompt for a password.
echo "Checking systemd units..."
UNIT_DIR="/home/tim/Repos/ProbeDataProcessor"
rsync -a ${UNIT_DIR}/probeDataProcessor.service ${UNIT_DIR}/probeDataProcessor.timer ${VM_USER}@${VM_HOST}:/tmp/
ssh -t ${VM_USER}@${VM_HOST} '
  if cmp -s /tmp/probeDataProcessor.service /etc/systemd/system/probeDataProcessor.service \
     && cmp -s /tmp/probeDataProcessor.timer /etc/systemd/system/probeDataProcessor.timer \
     && systemctl is-enabled --quiet probeDataProcessor.timer; then
    echo "systemd units up to date"
    rm -f /tmp/probeDataProcessor.service /tmp/probeDataProcessor.timer
  else
    echo "Installing systemd units..."
    sudo mv /tmp/probeDataProcessor.service /tmp/probeDataProcessor.timer /etc/systemd/system/ \
      && sudo systemctl daemon-reload \
      && sudo systemctl enable --now probeDataProcessor.timer
  fi'

echo "Restarting service..."
# probeDataProcessor.service is Type=oneshot (ProcessTemperatureData runs to completion and
# exits), so this triggers a fresh run of the job against the newly-deployed code rather than
# restarting a long-running daemon.
ssh -t ${VM_USER}@${VM_HOST} "sudo systemctl restart ${SERVICE_NAME}"

echo "Done."
