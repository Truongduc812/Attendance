FROM node:20-alpine
WORKDIR /app
COPY ["src/SourceBaseBE.SocketIOServer/package*.json", "."]
RUN npm install
COPY ["src/SourceBaseBE.SocketIOServer/", "."]
EXPOSE 2700
CMD node server/server.js