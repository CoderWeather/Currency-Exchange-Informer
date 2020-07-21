create database currency_exchange_db collate Cyrillic_General_CI_AS
go

create table Currencies
(
	original_name nvarchar(64) not null,
	engName varchar(64) not null,
	nominal int not null,
	parent_code varchar(7) not null,
	iso_num_code int,
	iso_char_code varchar(3),
	item_id varchar(7) not null
		constraint Currencies_pk
			primary key nonclustered
)
go

create table CurrencyRates
(
	item_id varchar(7) not null
		constraint CurrencyRates_Currencies_item_id_fk
			references Currencies,
	nominal int not null,
	value decimal(10,4) not null,
	date date not null,
	constraint CurrencyRates_pk
		primary key nonclustered (item_id, date)
)
go


