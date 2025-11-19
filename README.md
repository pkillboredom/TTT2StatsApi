# TTT2StatsApi

An ASP.NET Core Web API for querying TTT2 (Trouble in Terrorist Town 2) statistics from a Garry's Mod SQLite database.

## Docker Support

This application is Docker-ready with multi-stage builds for efficient containerization.

### Building the Docker Image

```bash
docker build -t ttt2stats-api .
```

### Running the Container

You need to specify the path to your `sv.db` file using the `--sv-path` argument:

```bash
docker run -d \
  -p 8080:8080 \
  -v /path/to/your/sv.db:/data/sv.db:ro \
  ttt2stats-api \
  --sv-path /data/sv.db
```

### Using Docker Compose

1. Edit `docker-compose.yml` and update the volume path to point to your `sv.db` file:
   ```yaml
   volumes:
     - /path/to/your/sv.db:/data/sv.db:ro
   ```

2. Start the service:
   ```bash
   docker-compose up -d
   ```

3. View logs:
   ```bash
   docker-compose logs -f
   ```

4. Stop the service:
   ```bash
   docker-compose down
   ```

### Environment Variables

- `ASPNETCORE_ENVIRONMENT`: Set to `Development` or `Production` (default: `Production`)
- `ASPNETCORE_URLS`: HTTP endpoint URLs (default: `http://+:8080`)

### Configuration

The application accepts the following command-line argument:
- `--sv-path`: Path to the Garry's Mod `sv.db` SQLite database file (required)

You can also set `SV_Path` in `appsettings.json` or through environment variables:
```bash
docker run -d \
  -p 8080:8080 \
  -e SV_Path=/data/sv.db \
  -v /path/to/your/sv.db:/data/sv.db:ro \
  ttt2stats-api
```

### API Documentation

Once running, access the Swagger UI at:
- Development: `http://localhost:8080/swagger`