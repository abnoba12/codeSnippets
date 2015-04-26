#Run this script at 2am
echo "Making a week backup of yesterday"
tar -pczf /mnt/backup/week/backup_"`date '+%m-%d-%Y'`".tar.gz /mnt/backup/today

echo "Deleting week backups older then 7 days"
find /mnt/backup/week/ -mtime +7 -exec rm {} \;

echo "Deleting yesterdays backup"
rm -rf /mnt/backup/today/*

echo "Making MySQL Backup"
mysqldump -u root -pLaNj2tnq4qEUnDyn --all-databases --routines --lock-tables=0| gzip > /mnt/backup/today/MySQLDB_"`date '+%m-%d-%Y'`".sql.gz

echo "Making Backup the SVN folders"
for repos in `ls /usr/local/svn/` ; do
	RESPOS_DIR=/usr/local/svn/$repos;
	echo "--Making backup of SVN repo: "$repos;
	svnadmin dump -q $RESPOS_DIR | gzip > /mnt/backup/today/"SVN_"$repos"_`date '+%m-%d-%Y'`".gz;
	chgrp backup $RESPOS_DIR;
done

echo "Making backup all files in the web directory"
tar -pczf /mnt/backup/today/weigandfamily_"`date '+%m-%d-%Y'`".tar.gz /var/www

echo "Backup webmin configs at 2:15am"
echo "Make abnoba12 the owner of all the backup files at 4:00am"
exit
