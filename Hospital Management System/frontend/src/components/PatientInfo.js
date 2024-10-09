import React, { useState, useEffect } from 'react';
import './PatientInfo.css';

function PatientInfo() {
  const [patients, setPatients] = useState([]);
  const [newPatient, setNewPatient] = useState({ name: '', address: '', mobile: '' });
  const [editPatient, setEditPatient] = useState(null);

  useEffect(() => {
    fetch('http://localhost:5000/api/patients')
      .then((res) => res.json())
      .then((data) => setPatients(data));
  }, []);

  const addPatient = () => {
    fetch('http://localhost:5000/api/patients', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(newPatient),
    })
      .then((res) => res.json())
      .then((data) => {
        setPatients([...patients, data]);
        setNewPatient({ name: '', address: '', mobile: '' });
      });
  };

  const deletePatient = (id) => {
    fetch(`http://localhost:5000/api/patients/${id}`, {
      method: 'DELETE',
    }).then(() => setPatients(patients.filter((patient) => patient._id !== id)));
  };

  const handleEditClick = (patient) => {
    setEditPatient(patient);
  };

  const updatePatient = () => {
    fetch(`http://localhost:5000/api/patients/${editPatient._id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(editPatient),
    })
      .then((res) => res.json())
      .then((updatedPatient) => {
        setPatients(
          patients.map((patient) =>
            patient._id === updatedPatient._id ? updatedPatient : patient
          )
        );
        setEditPatient(null);
      });
  };

  return (
    <div className="patient-info">
      <h2>Manage Patient Information</h2>

      <div className="add-patient-form">
        <input
          type="text"
          placeholder="Name"
          value={newPatient.name}
          onChange={(e) => setNewPatient({ ...newPatient, name: e.target.value })}
        />
        <input
          type="text"
          placeholder="Address"
          value={newPatient.address}
          onChange={(e) => setNewPatient({ ...newPatient, address: e.target.value })}
        />
        <input
          type="text"
          placeholder="Mobile"
          value={newPatient.mobile}
          onChange={(e) => setNewPatient({ ...newPatient, mobile: e.target.value })}
        />
        <button onClick={addPatient}>Add Patient</button>
      </div>

      <ul className="patient-list">
        {patients.map((patient) => (
          <li key={patient._id}>
            {patient.name} - {patient.address} - {patient.mobile}
            <button onClick={() => handleEditClick(patient)}>Edit</button>
            <button onClick={() => deletePatient(patient._id)}>Delete</button>
          </li>
        ))}
      </ul>

      {editPatient && (
        <div className="edit-patient-form">
          <input
            type="text"
            placeholder="Name"
            value={editPatient.name}
            onChange={(e) => setEditPatient({ ...editPatient, name: e.target.value })}
          />
          <input
            type="text"
            placeholder="Address"
            value={editPatient.address}
            onChange={(e) => setEditPatient({ ...editPatient, address: e.target.value })}
          />
          <input
            type="text"
            placeholder="Mobile"
            value={editPatient.mobile}
            onChange={(e) => setEditPatient({ ...editPatient, mobile: e.target.value })}
          />
          <button onClick={updatePatient}>Update</button>
          <button onClick={() => setEditPatient(null)}>Cancel</button>
        </div>
      )}
    </div>
  );
}

export default PatientInfo;
