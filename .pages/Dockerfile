# Build aşaması
FROM node:18-alpine as build-stage

# Çalışma dizinini belirleme
WORKDIR /app

# Package.json ve package-lock.json dosyalarını kopyalama
COPY package*.json ./

# Bağımlılıkları yükleme
RUN npm install

# Proje dosyalarını kopyalama
COPY . .

# Projeyi build etme
RUN npm run build

# Production aşaması
FROM nginx:stable-alpine as production-stage

# Build dosyalarını nginx'e kopyalama
COPY --from=build-stage /app/dist /usr/share/nginx/html

# 4398 portunu dışarı açma
EXPOSE 4398

# Nginx'i başlatma
CMD ["nginx", "-g", "daemon off;"]