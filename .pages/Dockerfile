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

# Nginx yapılandırma dosyasını kopyalama
COPY nginx.conf /etc/nginx/nginx.conf

# Build dosyalarını nginx'e kopyalama
COPY --from=build-stage /app/dist /usr/share/nginx/html

# 4389 portunu dışarı açma
EXPOSE 4389

# Nginx'i başlatma
CMD ["nginx", "-g", "daemon off;"]