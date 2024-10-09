const express = require('express');
const mongoose = require('mongoose');
const cors = require('cors');
const bodyParser = require('body-parser');

const app = express();
app.use(cors());
app.use(bodyParser.json());

mongoose.connect('mongodb+srv://harshj:harshjariya490@cluster0.ii8cv.mongodb.net/Hospital', { useNewUrlParser: true, useUnifiedTopology: true }).then(console.log("MON. Db Connect..."));

const patientRoutes = require('./routes/patient');
app.use('/api/patients', patientRoutes);

app.listen(5000, () => console.log('Server running on port 5000'))