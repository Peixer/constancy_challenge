## Brick Abode Challenge - F#

Brick Abode Tech Challenge in F# - API for broker 

The purpose of the challenge was to create an API for broker, where can access users and providers information.

- I choose to create three projects WebApp, Core and Migrations.
  
- WebApp
    - Controllers using Rest API naming patterns
    - Validation input routes

- Core
    - Use of Repository, Models, Validation
    - Dapper to integrate with postgres database

- Migrations
  - Migrations to database

### How to run

1. Clone the repository
2. Run docker-compose up -d


### Postman Doc

You can access entire routes inside the [postman file](https://github.com/Peixer/constancy_challenge/blob/master/BrickAbode.postman_collection.json)


