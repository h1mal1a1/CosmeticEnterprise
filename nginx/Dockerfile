# syntax=docker/dockerfile:1.4

FROM node:20-alpine AS frontend-build
WORKDIR /app

COPY --from=frontend_src package*.json ./
RUN npm install

COPY --from=frontend_src . .
RUN npm run build

FROM nginx:1.27-alpine AS final

COPY nginx/nginx.conf /etc/nginx/nginx.conf
COPY nginx/certs /etc/nginx/certs
COPY --from=frontend-build /app/dist /usr/share/nginx/html

EXPOSE 80 443

CMD ["nginx", "-g", "daemon off;"]