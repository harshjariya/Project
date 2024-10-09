const express = require('express');
const router = express.Router();
const Patient = require('../models/Patient');

// GET all patients
router.get('/', async (req, res) => {
  const patients = await Patient.find();
  res.json(patients);
});

// POST a new patient
router.post('/', async (req, res) => {
  const { name, address, mobile } = req.body;
  const newPatient = new Patient({ name, address, mobile });
  await newPatient.save();
  res.json(newPatient);
});

// PUT (update) a patient
router.put('/:id', async (req, res) => {
  const { name, address, mobile } = req.body;
  const updatedPatient = await Patient.findByIdAndUpdate(req.params.id, { name, address, mobile }, { new: true });
  res.json(updatedPatient);
});

// DELETE a patient
router.delete('/:id', async (req, res) => {
  await Patient.findByIdAndDelete(req.params.id);
  res.json({ message: 'Patient deleted' });
});

module.exports = router;
