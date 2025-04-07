#build
docker build -t ct-postgres-db:test .

#run
docker run --name ct-postgres-db -it -p 5432:5432 --restart always ct-postgres-db:test