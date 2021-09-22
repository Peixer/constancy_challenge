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

### How to run - Docker Compose

1. Clone the repository
2. Run docker-compose up -d

### How to run - Without Docker Compose

1. Clone the repository
2. dotnet tool restore
3. dotnet paket install
4. docker run --name postgres-db -e POSTGRES_PASSWORD=WAhBRV2qHNA9c8yd744zH2w4 -p 5432:5432 -d postgres
5. dotnet saturn migration
6. dotnet fake build -t Run



### Seeds

To generate seeds of Users and Providers, execute the following command:

- sudo bash seeds.sh

### Postman Doc

You can access entire routes inside the [postman file](https://github.com/Peixer/constancy_challenge/blob/master/BrickAbode.postman_collection.json)


### Future Implementation

- [ ] Use Pagination in GET routes
- [ ] Handle error
- [ ] Add route to filter histories
- [ ] Use ORM
- [ ] Unit test
- [ ] Setup CI/CD
- [ ] Add JSON API feature
- [ ] JWT Authentication
- [ ] Swagger Documentation
