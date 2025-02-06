import React from 'react';
import { Link } from 'react-router-dom';
import './TutorialAdmin.css';

const TutorialAdmin = () => {
  return (
    <div id="tutorial-admin-container">
      <div id="tutorial-admin-header">
        <div id="tutorial-admin-nav-buttons">
          
        <Link to="/tutorial" id="tutorial-recipient-nav-button" className={window.location.pathname === '/tutorial' ? 'active' : ''}>
            Tutorial (Main)
          </Link>
          <Link to="/tutorial/recipient" id="tutorial-recipient-nav-button" className={window.location.pathname === '/tutorial/recipient' ? 'active' : ''}>
            Recipient Tutorial 
          </Link>
          <Link to="/tutorial/staff" id="tutorial-recipient-nav-button" className={window.location.pathname === '/tutorial/staff' ? 'active' : ''}>
            Staff Tutorial
          </Link>
          <Link to="/tutorial/admin" id="tutorial-recipient-nav-button" className={window.location.pathname === '/tutorial/admin' ? 'active' : ''}>
            Admin Tutorial 
          </Link>
        </div>
      </div>

      <div id="tutorial-admin-content">
        <h1 id="tutorial-admin-title">Admin Tutorial</h1>

        <section id="tutorial-admin-getting-started">
          <div className="tutorial-section-content">
            <h3>Full System Access</h3>
            <ul>
              <li>As an admin, you have access to all features available to Recipients and Staff</li>
            </ul>

            <h3>Managing Pantries</h3>
            <img src="../tutorial_admin_image/pantry.png" alt="Pantry Management" />
            <ul>
              <li>Navigate to the Pantry Management section</li>
              <li>Add or update pantry details such as name and access codes</li>
              <li>Oversee multiple pantry branches and ensure they are functioning efficiently</li>
            </ul>
            
            <div className="event-image-container">
              <img src="../tutorial_admin_image/newpantry.png" alt="Pantry Request" />
              <p>Email sent to the admin when a new pantry is requested</p>
            </div>
          </div>
        </section>
      </div>
    </div>
  );
};

export default TutorialAdmin;
