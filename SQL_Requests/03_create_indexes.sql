drop index if exists movie_actor_idx;
drop index if exists movie_genre_idx;
drop index if exists movie_collection_idx;
drop index if exists watchlist_status_idx;
drop index if exists review_idx;

create index movie_actor_idx on movie_actor(actor_id);

create index movie_genre_idx on movie_genre(genre_id);

create index movie_collection_idx on movie_in_collection(collection_id);

create index watchlist_status_idx on watch_list (user_id, status, added_at desc);

create index review_idx on review (movie_id, updated_at desc);



-- EXPLAIN ANALYZE SELECT * FROM watch_list WHERE user_id = 1; 

analyze;