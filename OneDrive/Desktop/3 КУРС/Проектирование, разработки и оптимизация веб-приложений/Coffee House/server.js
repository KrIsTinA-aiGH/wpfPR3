const http = require('http');
const fs = require('fs');
const path = require('path');

const PORT = 3000;
const HOST = '0.0.0.0';

const mimeTypes = {
  '.html': 'text/html',
  '.css': 'text/css',
  '.js': 'text/javascript',
  '.png': 'image/png',
  '.jpg': 'image/jpeg',
  '.gif': 'image/gif',
  '.ico': 'image/x-icon',
  '.svg': 'image/svg+xml',
  '.json': 'application/json'
};

const server = http.createServer((req, res) => {
  console.log(`Запрос: ${req.url}`);
  
  // Убираем параметры запроса
  let filePath = req.url.split('?')[0];
  
  // Если корневой путь, отдаем index.html
  if (filePath === '/') {
    filePath = '/index.html';
  }
  
  // Убираем начальный слэш
  filePath = filePath.substring(1);
  
  // Если файл не указан, ищем index.html
  if (filePath === '') {
    filePath = 'index.html';
  }
  
  const fullPath = path.join(__dirname, filePath);
  const ext = path.extname(fullPath);
  const contentType = mimeTypes[ext] || 'text/plain';

  fs.readFile(fullPath, (err, content) => {
    if (err) {
      console.log(`Файл не найден: ${filePath}`);
      // Пробуем отдать index.html для SPA маршрутов
      fs.readFile(path.join(__dirname, 'index.html'), (err, content) => {
        if (err) {
          res.writeHead(404);
          res.end('File not found');
        } else {
          res.writeHead(200, { 'Content-Type': 'text/html' });
          res.end(content, 'utf-8');
        }
      });
    } else {
      res.writeHead(200, { 'Content-Type': contentType });
      res.end(content, 'utf-8');
    }
  });
});

server.listen(PORT, HOST, () => {
  console.log(`🚀 Сервер запущен на http://${HOST}:${PORT}`);
  console.log(`📁 Корневая директория: ${__dirname}`);
  console.log(`🌐 Откройте браузер: http://localhost:${PORT}`);
});