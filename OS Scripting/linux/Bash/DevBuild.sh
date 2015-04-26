#!/bin/bash
#Append output and stderr to stdout and to a file: 2>&1 | tee -a buildlog.txt
sudo echo "------------------------------------------------" 2>&1 | tee -a buildlog.txt
sudo echo "Build Started on $(date '+%m-%d-%Y %T') by $USER" 2>&1 | tee -a buildlog.txt
sudo echo "---Step 1---" 2>&1 | tee -a buildlog.txt
sudo echo "Update SVN" 2>&1 | tee -a buildlog.txt
sudo svn update --username $USER /usr/local/wcsr-build 2>&1 | tee -a buildlog.txt
sudo echo "---Step 2---" 2>&1 | tee -a buildlog.txt
sudo echo "backup WCSR to /usr/local/wcsr-axiom/apps/WCSRAxiomstackDevBackup.tar.gz" 2>&1 | tee -a buildlog.txt
sudo tar -C /usr/local/wcsr-axiom/apps/ -pczf /usr/local/wcsr-axiom/apps/WCSRAxiomstackDevBackup.tar.gz wcsr 2>&1 | tee -a buildlog.txt
sudo echo "---Step 3---" 2>&1 | tee -a buildlog.txt
sudo echo "sync build dir with live dir" 2>&1 | tee -a buildlog.txt
sudo rsync -vur --exclude=.svn /usr/local/wcsr-build/ /usr/local/wcsr-axiom/apps/wcsr 2>&1 | tee -a buildlog.txt
sudo echo "Build Completed at $(date '+%m-%d-%Y %T')" 2>&1 | tee -a buildlog.txt
sudo echo ""  2>&1 | tee -a buildlog.txt