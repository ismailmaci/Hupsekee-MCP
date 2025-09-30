# Build stage
FROM eclipse-temurin:21-jdk-alpine AS builder

WORKDIR /build

# Copy Maven wrapper and dependencies
COPY mvnw mvnw.cmd pom.xml ./
COPY .mvn .mvn
RUN chmod +x mvnw

# Download dependencies for better caching
RUN ./mvnw dependency:go-offline -B

# Copy and build application
COPY src ./src
RUN ./mvnw clean package -DskipTests

# Runtime stage
FROM eclipse-temurin:21-jre-alpine

# Create non-root user for security
RUN addgroup -g 1001 -S chess && \
    adduser -u 1001 -S chess -G chess

WORKDIR /app

# Copy only the built JAR from build stage
COPY --from=builder /build/target/mcp_chess_demo-0.0.1-SNAPSHOT.jar app.jar

# Create logs directory with proper ownership
RUN mkdir -p /app/logs && chown -R chess:chess /app

# Switch to non-root user
USER chess

# Run the application
CMD ["java", "-jar", "app.jar"]