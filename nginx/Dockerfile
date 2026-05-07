# syntax=docker/dockerfile:1.4

FROM node:20-alpine AS frontend-build
WORKDIR /app

COPY --from=frontend_src package*.json ./
RUN npm install

COPY --from=frontend_src . .
RUN npm run build

FROM node:20-alpine AS admin-frontend-build
WORKDIR /app

COPY --from=admin_frontend_src package*.json ./
RUN npm install

COPY --from=admin_frontend_src . .
RUN npm run build

FROM nginx:1.27-alpine AS final

ARG NGINX_CONF=nginx.prod.conf

COPY nginx/${NGINX_CONF} /etc/nginx/nginx.conf

COPY --from=frontend-build /app/dist /usr/share/nginx/html
COPY --from=admin-frontend-build /app/dist /usr/share/nginx/admin

EXPOSE 80 443

CMD ["nginx", "-g", "daemon off;"]