#Run this script monthly at 3am
echo "Remove yearly backups older than 5 years"
find /mnt/backup/year/ -mtime +1825 -exec rm {} \;

echo "Remove monthly backups older than 1 year"
find /mnt/backup/month/ -mtime +365 -exec rm {} \;

echo "Make the monthly backup"
tar -pczf /mnt/backup/month/backup_"`date '+%m-%d-%Y'`".tar.gz /mnt/backup/today
exit