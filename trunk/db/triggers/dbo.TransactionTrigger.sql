set ansi_nulls on
set quoted_identifier on
go

if exists (select * from sys.triggers where object_id = OBJECT_ID(N'[dbo].[TR_TRANSACTION_HISTORY]'))
	drop trigger [dbo].[TR_TRANSACTION_HISTORY]
go

create trigger [dbo].[TR_TRANSACTION_HISTORY]
on [dbo].[TRANSACTION]
after insert, update, delete
as
begin
	insert into [dbo].[TRANSACTION_HISTORY]
	(
		TRANSACTION_ID,
		UNIQUE_ID,
		ACCOUNT_ID,
		[DATE],
		[DESCRIPTION],
		AMOUNT,
		[STATUS],
		MODIFIED
	)
	select
		TRANSACTION_ID,
		UNIQUE_ID,
		ACCOUNT_ID,
		[DATE],
		[DESCRIPTION],
		AMOUNT,
		[STATUS],
		GETDATE()
	from deleted
end
go