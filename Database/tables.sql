use gtechapp_SpidySense
go

-- sp_tables

print '--------------------------------------------------------------------------'
print 'Drop all foreign keys for all tables'
if exists(select 1 from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS)
begin
	declare @tblProd table(rowNum int, constraint_name nvarchar(50), table_name nvarchar(50))

	-- get all constraints	
	insert into @tblProd(rowNum, constraint_name, table_name)
	select  ROW_NUMBER() OVER(order by rc.constraint_name) rownumber, rc.CONSTRAINT_NAME, ctu.TABLE_NAME
	from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc
	inner join INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE ctu on ctu.CONSTRAINT_NAME = rc.CONSTRAINT_NAME

	-- loop thru constraints and drop them
	declare @nextRow int, @nextConstraintName nvarchar(50), @nextTableName nvarchar(50), @sql nvarchar(max)
	select top 1 @nextRow=rowNum, @nextConstraintName=constraint_name, @nextTableName=table_name from @tblProd order by rowNum
	while(@nextRow <= (select COUNT(*) from @tblProd))
	begin
		select @nextConstraintName=constraint_name, @nextTableName=table_name from @tblProd where rowNum=@nextRow
		exec('alter table ' + @nextTableName + ' drop constraint ' + @nextConstraintName)
		set @nextRow = @nextRow + 1
	end
end
go

print '--------------------------------------------------------------------------'
print 'Table: Website'
if exists(select 1 from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Website')
begin
	drop table dbo.Website
end
go
create table dbo.Website
(
	Id int not null identity(1,1) primary key,
	[Name] nvarchar(20) not null,
	SearchUrl nvarchar(250) null
)
insert into dbo.Website values('ETickets', 'etickets.com');
insert into dbo.Website values('Google', 'google.com');
insert into dbo.Website values('Yelp', 'yelp.com');
go

print '--------------------------------------------------------------------------'
print 'Table: Website'
if exists(select 1 from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Website')
begin
	drop table dbo.Website
end
go
create table dbo.Website
(
	Id int not null identity(1,1) primary key,
	[Name] nvarchar(20) not null,
	[Description] nvarchar(1000) null,
	[Url] nvarchar(500) null
)
insert into dbo.Website values('ETickets', 'Free website with events.', 'etickets.com');
insert into dbo.Website values('Google', 'Biggest search engine', 'google.com');
insert into dbo.Website values('Yelp', 'Biggest review site for all kinds of businesses.', 'yelp.com');
go

print '--------------------------------------------------------------------------'
print 'Table: WebUrl'
if exists(select 1 from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'WebUrl')
begin
	drop table dbo.WebUrl
end
go
create table dbo.WebUrl
(
	Id int not null identity(1,1) primary key,
	WebsiteId int not null foreign key references Website(Id),
	[Name] nvarchar(50) not null,
	[Url] nvarchar(500) not null,
	RowSelector nvarchar(50) not null
)
go

print '--------------------------------------------------------------------------'
print 'Table: WebUrlField'
if exists(select 1 from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'WebUrlField')
begin
	drop table dbo.WebUrlField
end
go
create table dbo.WebUrlField
(
	Id int not null identity(1,1) primary key,
	WebUrlId int not null foreign key references WebUrl(Id),
	[Name] nvarchar(50) not null,
	Selector nvarchar(50) not null
)
go

/*
sp_tables

select *
from website w
left join webUrl wu on wu.websiteId = w.Id
left join webUrlField wuf on wuf.webUrlId = wu.Id


*/