FROM node:current-alpine As node

WORKDIR /usr/src/app

COPY ToDo.Web.SPA .

RUN npm install

RUN npm run build:prod

FROM nginx:latest

EXPOSE 80

COPY --from=node /usr/src/app/dist/ToDo-Web-SPA /usr/share/nginx/html
