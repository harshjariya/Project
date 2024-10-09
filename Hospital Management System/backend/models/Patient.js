const mongoose = require('mongoose');

const PatientSchema = new mongoose.Schema({
  name: { type: String, required: true },
  address: { type: String, required: true },
  mobile: { type: String, required: true },
});

const Patient = mongoose.model('Patient', PatientSchema);
module.exports = Patient;
