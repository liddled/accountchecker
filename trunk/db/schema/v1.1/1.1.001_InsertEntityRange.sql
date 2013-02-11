insert into SCRIPT values ('V1.1_001', 'Added ENTITY_ID_RANGE values from ACCOUNT,TRANSACTION,CATEGORY,BUDGET,ALLOWANCE', getdate())
go

insert into ENTITY_ID_RANGE values (1,'ACCOUNT',1,10,1,1)
insert into ENTITY_ID_RANGE values (2,'TRANSACTION',1,1000000,1,1)
insert into ENTITY_ID_RANGE values (3,'CATEGORY',1,500,1,1)
insert into ENTITY_ID_RANGE values (4,'BUDGET',1,20,1,1)
insert into ENTITY_ID_RANGE values (5,'ALLOWANCE',1,10000,1,1)
go