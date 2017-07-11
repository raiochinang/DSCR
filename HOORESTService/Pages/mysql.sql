/* Doctor */
select id, full_name_fld from hoo.hoo_user_vw as A
join hoo.users_roles as B
on A.id = B.uid
where rid = 4;

/* Nurse */
select id, full_name_fld from hoo.hoo_user_vw as A
join hoo.users_roles as B
on A.id = B.uid
where rid = 10;

/* Particular */
select id, brand_code_fld, item_name_fld, item_price_fld, item_gross_amount_fld, item_retention_fld from hoo.hoo_item_tbl
where id = 88;


