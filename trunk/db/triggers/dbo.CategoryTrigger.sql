set ansi_nulls on
set quoted_identifier on
go

if exists (select * from sys.triggers where object_id = OBJECT_ID(N'[dbo].[TR_CATEGORY_HISTORY]'))
	drop trigger [dbo].[TR_CATEGORY_HISTORY]
go

create trigger [dbo].[TR_CATEGORY_HISTORY]
on [dbo].[CATEGORY]
after insert, update, delete
as
begin
	insert into [dbo].[CATEGORY_HISTORY]
	(
		CATEGORY_ID,
		CATEGORY_NAME,
		[STATUS],
		MODIFIED
	)
	select
		CATEGORY_ID,
		CATEGORY_NAME,
		[STATUS],
		GETDATE()
	from deleted
end
go