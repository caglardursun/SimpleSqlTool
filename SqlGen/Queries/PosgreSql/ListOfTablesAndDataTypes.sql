SELECT attrelid::regclass AS tbl
     , attname            AS col
     , atttypid::regtype  AS datatype
     , atttypid::
       -- more attributes?
FROM   pg_attribute
WHERE  attrelid = 'public.company'::regclass  -- table name, optionally schema-qualified
AND    attnum > 0
AND    NOT attisdropped
ORDER  BY attnum;