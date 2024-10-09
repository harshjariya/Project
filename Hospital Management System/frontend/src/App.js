import React from 'react';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';
import Home from './components/Home';
import PatientInfo from './components/PatientInfo';
import AboutUs from './components/AboutUs';
import './styles/App.css';
import './styles/Navbar.css';

function App() {
  return (
    <Router>
      <header className="navbar">
        <div className="navbar-container">
          <h1 className="navbar-logo">MERN Hospital</h1>
          <input type="checkbox" className="navbar-toggle" id="navbar-toggle" />
          <label htmlFor="navbar-toggle" className="navbar-icon">
            <span></span>
            <span></span>
            <span></span>
          </label>
          <nav className="navbar-links">
            <ul>
              <li><Link to="/">Home</Link></li>
              <li><Link to="/patients">Patient Info</Link></li>
              <li><Link to="/about">About Us</Link></li>
            </ul>
          </nav>
        </div>
      </header>

      <main>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/patients" element={<PatientInfo />} />
          <Route path="/about" element={<AboutUs />} />
        </Routes>
      </main>

      <footer>
        <p>&copy; 2024 MERN Hospital. All rights reserved.</p>
      </footer>
    </Router>
  );
}

export default App;
