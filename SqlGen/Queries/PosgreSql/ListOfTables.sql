SELECT table_schema,table_name
FROM information_schema.tables where table_schema = 'public'
ORDER BY table_schema,table_name;