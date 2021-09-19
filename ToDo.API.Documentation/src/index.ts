import SwaggerUI from 'swagger-ui'
import 'swagger-ui/dist/swagger-ui.css';

const spec = require('./swagger/swagger.yml');

SwaggerUI({
    spec,
    dom_id: '#swagger-ui',
    deepLinking: true,
    presets: [
        SwaggerUI.presets.apis,
        SwaggerUI.SwaggerUIStandalonePreset
    ],
    plugins: [
        SwaggerUI.plugins.DownloadUrl
    ],
})