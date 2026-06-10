drop table if exists users cascade;
drop table if exists review cascade;
drop table if exists watch_list cascade;
drop table if exists movie cascade;
drop table if exists movie_genre cascade;
drop table if exists genre cascade;
drop table if exists category cascade;
drop table if exists movie_actor cascade;
drop table if exists actor cascade;

drop type if exists watch_status;
drop type if exists category_type;

create type watch_status as enum ('watched', 'watching', 'planning');
create type category_type as enum ('movie', 'tv_series', 'anime', 'cartoon');

create table users (
	user_id serial primary key,
	username varchar(20) not null
);

create table genre (
	genre_id serial primary key,
	genre_name varchar(20) not null unique
);

create table actor (
	actor_id serial primary key,
	actor_name varchar(50) not null
);


create table movie (
	movie_id serial primary key,
	movie_name varchar(300) not null,
	release_date date not null,
	category category_type not null
);

create table movie_genre (
	movie_genre_id serial primary key,
	movie_id int references movie(movie_id) on delete cascade,
	genre_id int references genre(genre_id) on delete cascade
);


create table movie_actor (
	movie_actor_id serial primary key,
	movie_id int references movie(movie_id) on delete cascade,
	actor_id int references actor(actor_id) on delete cascade
);

create table review (
	review_id serial primary key,
	rating int not null check (rating >= 0 and rating <=10),
	description text,
	created_at timestamp default current_timestamp not null,
	updated_at timestamp default current_timestamp,
	user_id int references users(user_id) on delete cascade,
	movie_id int references movie(movie_id) on delete cascade
);

create table watch_list (
	watch_list_id serial primary key,
	status watch_status not null,
	added_at timestamp default current_timestamp not null,
	user_id int references users(user_id) on delete cascade,
	movie_id int references movie(movie_id) on delete cascade
);


