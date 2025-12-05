FROM prom/prometheus:latest

# El contexto va a ser: WebApi/Docker
# y ahí existe la carpeta: prometheus/prometheus.yml
COPY prometheus/prometheus.yml /etc/prometheus/prometheus.yml