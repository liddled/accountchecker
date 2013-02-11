set ansi_nulls on
set quoted_identifier on
go

if exists (select * from sys.triggers where object_id = OBJECT_ID(N'[dbo].[TR_ACCOUNT_HISTORY]'))
	drop trigger [dbo].[TR_ACCOUNT_HISTORY]
go

create trigger [dbo].[TR_ACCOUNT_HISTORY]
on [dbo].[ACCOUNT]
after insert, update, delete
as
begin
	insert into [dbo].[ACCOUNT_HISTORY]
	(
		ACCOUNT_ID,
		BANK_NAME,
		BANK_CODE,
		CARD_NUMBER,
		LOCALE,
		[STATUS],
		MODIFIED
	)
	select
		ACCOUNT_ID,
		BANK_NAME,
		BANK_CODE,
		CARD_NUMBER,
		LOCALE,
		[STATUS],
		GETDATE()
	from deleted
end
go