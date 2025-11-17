# RobotStats - Мониторинг автоматизированных процессов

Система для сбора статистики и мониторинга работы автоматизированных процессов (роботов).

##  Архитектура

- **Backend**: ASP.NET Core 8 Web API
- **Frontend**: HTML + JavaScript (статический)
- **Database**: PostgreSQL
- **Reverse Proxy**: Nginx с HTTPS
- **Containerization**: Docker + Docker Compose

##  Быстрый запуск (разработка)

git clone <repository-url>
cd RobotStats
docker-compose up --build
Доступные endpoints:

Frontend: https://localhost

API Documentation: http://localhost:5000/swagger

Health Check: http://localhost:5000/health

 Продакшен развертывание на Linux
Предварительные требования
Сервер Linux

Доменное имя, настроенное на IP сервера

Доступ по SSH с правами sudo

1. Базовая настройка сервера
bash
# Обновление системы
sudo apt update && sudo apt upgrade -y

# Установка Docker
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh

# Установка Docker Compose
sudo apt install docker-compose-plugin -y

# Настройка прав доступа
sudo usermod -aG docker $USER
newgrp docker

# Настройка фаервола
sudo ufw allow 22    # SSH
sudo ufw allow 80    # HTTP
sudo ufw allow 443   # HTTPS
sudo ufw enable
2. Подготовка проекта
bash
# Клонирование репозитория
git clone <repository-url>
cd RobotStats

# Создание необходимых директорий
mkdir -p nginx/ssl
3. Получение SSL сертификатов
bash
# Установка certbot
sudo apt install certbot -y

# Остановка сервисов для получения сертификата
docker-compose down

# Получение сертификата (замените example.com на ваш домен)
sudo certbot certonly --standalone -d example.com -d www.example.com

# Копирование сертификатов
sudo cp /etc/letsencrypt/live/example.com/fullchain.pem nginx/ssl/cert.pem
sudo cp /etc/letsencrypt/live/example.com/privkey.pem nginx/ssl/key.pem
sudo chown $USER:$USER nginx/ssl/*.pem

# Настройка прав доступа
sudo chmod 644 nginx/ssl/cert.pem
sudo chmod 600 nginx/ssl/key.pem
4. Конфигурация приложения
Создание файла окружения

cat > .env << EOF
POSTGRES_DB=robotstats
POSTGRES_USER=postgres
POSTGRES_PASSWORD=your_secure_password_here
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=Host=postgres;Database=robotstats;Username=postgres;Password=your_secure_password_here
EOF

Критическое исправление nginx.conf ( тк если не исправлять, проект использует самоподписанные сертификаты ssl)

nano nginx/nginx.conf
Замените:

nginx
# БЫЛО:
server_name localhost;
ssl_certificate /etc/nginx/ssl/cert.pem;
ssl_certificate_key /etc/nginx/ssl/key.pem;

# СТАЛО:
server_name example.com www.example.com;
ssl_certificate /etc/letsencrypt/live/example.com/fullchain.pem;
ssl_certificate_key /etc/letsencrypt/live/example.com/privkey.pem;

5. Запуск приложения
bash
docker-compose up -d
docker-compose ps
docker-compose logs -f
6. Автоматическое обновление SSL
bash
cat > update-ssl.sh << 'EOF'
#!/bin/bash
cd /home/$(whoami)/RobotStats
docker-compose stop nginx
certbot renew
cp /etc/letsencrypt/live/example.com/fullchain.pem ./nginx/ssl/cert.pem
cp /etc/letsencrypt/live/example.com/privkey.pem ./nginx/ssl/key.pem
docker-compose start nginx
echo "SSL updated: $(date)"
EOF

chmod +x update-ssl.sh

# Добавление в cron
(crontab -l 2>/dev/null; echo "0 3 * * * /home/$(whoami)/RobotStats/update-ssl.sh >> /home/$(whoami)/ssl-renew.log 2>&1") | crontab -
API Endpoints
GET /api/dashboard - Метрики дашборда

GET /api/robotstats - Список запусков роботов

POST /api/robotstats - Добавление нового запуска

GET /health - Health check

Пример добавления данных
curl -X POST https://example.com/api/robotstats \
  -H "Content-Type: application/json" \
  -d '{
    "robotName": "DataProcessor",
    "status": "Success", 
    "timeSavedMinutes": 30
  }'

При возникновении проблем:

Проверьте логи: docker-compose logs

Убедитесь что домен настроен правильно

Проверьте доступность портов 80 и 443

Убедитесь что SSL сертификаты действительны