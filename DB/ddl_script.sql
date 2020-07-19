create database currency_exchange_db collate Cyrillic_General_CI_AS
go

create table Currencies
(
	original_name nvarchar(32) not null,
	engName varchar(32) not null,
	nominal int not null,
	parent_code varchar(7) not null,
	iso_num_code int not null,
	iso_char_code varchar(3) not null,
	item_id varchar(7) not null
		constraint Currencies_pk
			primary key nonclustered
)
go

create table CurrencyConversions
(
	item_id varchar(7) not null
		constraint CurrencyConversions_pk
			primary key nonclustered
		constraint CurrencyConversions_Currencies_item_id_fk
			references Currencies,
	nominal int not null,
	value decimal(18) not null,
	date date not null
)
go
