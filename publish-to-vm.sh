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
ssh -t ${VM_USER}@${VM_HOST} "sudo mkdir -p ${VM_DEPLOY_DIR} && sudo chown ${VM_USER}: ${VM_DEPLOY_DIR}"

echo "Copying to VM..."
rsync -avz --delete ./publish/ ${VM_USER}@${VM_HOST}:${VM_DEPLOY_DIR}/

echo "Restarting service..."
# probeDataProcessor.service is Type=oneshot (ProcessTemperatureData runs to completion and
# exits), so this triggers a fresh run of the job against the newly-deployed code rather than
# restarting a long-running daemon.
ssh -t ${VM_USER}@${VM_HOST} "sudo systemctl restart ${SERVICE_NAME}"

echo "Done."
