

# USERS
for i in {1..3}; do \
  curl -X POST -H "Accept: application/json" -H "Content-Type: application/json" \
    -d '{"name": "Teste '$RANDOM'"}' "http://localhost:8085/api/users"; \
done


# PROVIDERS
for i in {1..3}; do \
  curl -X POST -H "Accept: application/json" -H "Content-Type: application/json" \
    -d '{"name": "Teste '$RANDOM'"}' "http://localhost:8085/api/providers"; \
done
