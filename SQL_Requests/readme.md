# SQL Requests

This folder contains SQL queries for working with the WatchDiary database.

## Query Types

- **SELECT queries** — retrieving data by various criteria (movies by genre, user reviews, viewing statistics, etc.)
- **INSERT/UPDATE queries** — adding and updating records (adding a movie, review, updating watch status, etc.)
- **DELETE queries** — removing records (deleting a review, removing a movie from the list, etc.)

## Notes

- Queries are optimized for PostgreSQL
- Uses custom types `watch_status` and `category_type` defined in the database schema
- All queries are tested against the structure defined in `02_createTables.sql`

## Quick Start

1. Create database: `01_createDataBase.sql`
2. Create tables: `02_createTables.sql`
3. Execute required queries from this folder
