drop table if exists users cascade;
drop table if exists review cascade;
drop table if exists watch_list_item cascade;
drop table if exists movie cascade;
drop table if exists movie_genre cascade;
drop table if exists genre cascade;
drop table if exists category cascade;
drop table if exists movie_actor cascade;
drop table if exists actor cascade;
drop table if exists collections cascade;
drop table if exists movie_in_collection cascade;

drop type if exists watch_status cascade;
drop type if exists category_type cascade;

create type watch_status as enum ('watched', 'watching', 'planning', 'paused', 'dropped');
create type category_type as enum ('movie', 'tv_series', 'anime', 'cartoon');

create table users (
	user_id serial primary key,
	username varchar(20) unique not null,
	user_password varchar(255) not null,
	user_email varchar(100) unique not null
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
	imdb_id varchar(20) null,
	shikimori_id int null,
	kinopoisk_id int null,
	rottentomato_id varchar(255) null,
	imdb_rating decimal null,
	tmdb_id int,
	shikimori_rating decimal null,
	rt_rating decimal null,
	movie_name varchar(300) not null,
	release_date date not null,
	category category_type not null,
	cover_url varchar(500) null,
	description text,
	full_plot text
);

create table movie_genre (
	movie_id int references movie(movie_id) on delete cascade,
	genre_id int references genre(genre_id) on delete cascade,
	primary key(movie_id, genre_id)
);

create table movie_actor (
	movie_id int references movie(movie_id) on delete cascade,
	actor_id int references actor(actor_id) on delete cascade,
	primary key(movie_id, actor_id)
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

create table watch_list_item (
	watch_list_id serial primary key,
	status watch_status not null,
	added_at timestamp default current_timestamp not null,
	user_id int references users(user_id) on delete cascade,
	movie_id int references movie(movie_id) on delete cascade,
	constraint uq_user_movie_watchlist unique (user_id, movie_id)
);

create table collections (
	collection_id serial primary key,
	collection_name varchar(100) not null,
	user_id int references users(user_id) on delete cascade,
	constraint uq_user_collection_name unique (user_id, collection_name)
);

create table movie_in_collection (
	movie_id int references movie(movie_id) on delete cascade,
	collection_id int references collections(collection_id) on delete cascade,
	primary key(movie_id, collection_id)
);


