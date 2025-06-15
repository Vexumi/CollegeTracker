# Stage 1: сборка Angular
FROM node:20 AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

# Stage 2: nginx для отдачи фронтенда + прокси
FROM nginx:alpine
COPY --from=build /app/dist/CollegeTracker.Angular/browser/ /usr/share/nginx/html/
COPY nginx.conf /etc/nginx/nginx.conf
