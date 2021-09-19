FROM node:current-alpine As node

WORKDIR /usr/src/app

COPY ToDo.API.Documentation .

RUN npm install

RUN npm run build

FROM nginx:latest

EXPOSE 80

COPY --from=node /usr/src/app/dist /usr/share/nginx/html