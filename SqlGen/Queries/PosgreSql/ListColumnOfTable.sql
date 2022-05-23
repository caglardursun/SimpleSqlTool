SELECT *
  FROM information_schema.columns
 WHERE table_schema = 'public' --Whatever you schema is
   AND table_name   = 'company';