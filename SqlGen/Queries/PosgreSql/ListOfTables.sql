SELECT table_schema,table_name
FROM information_schema.tables where table_schema = 'public' --Whatever you schema is
ORDER BY table_schema,table_name;