select name
  from DBname.sys.tables
 where name like '%xxx%'
   and is_ms_shipped = 0
