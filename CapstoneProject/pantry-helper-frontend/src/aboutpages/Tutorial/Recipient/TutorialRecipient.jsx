import React from 'react';
import { Link } from 'react-router-dom';
import './TutorialRecipient.css';

const TutorialRecipient = () => {
  return (
    <div id="tutorial-recipient-container">
      <div id="tutorial-recipient-header">
        <div id="tutorial-recipient-nav-buttons">
          
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

      <div id="tutorial-recipient-content">
        <h1 id="tutorial-recipient-title">Recipient Tutorial</h1>

        <section id="tutorial-recipient-getting-started">
          <div className="tutorial-section-content">
            <h3>Login</h3>
            <ul>
              <li>Use Google login to access your account</li>
            </ul>

            <h3>Manage Subscriptions</h3>
            
            <img src="../tutorial_recipient_image/managesubscription.png" alt="Manage Subscriptions" />
            <ul>
              <li>Go to the Manage Subscriptions section in Profile page to subscribe to your pantry</li>
              <li>If you have selected multiple pantries, you can switch between pantries using the navbar</li>
            </ul>

            <h3>Dashboard Overview</h3>
            <img src="../tutorial_recipient_image/dashboard.png" alt="Dashboard" />
            <ul>
              <li>After logging in, you'll land on your Dashboard</li>
              <li>The dashboard provides key information, including:</li>
              <ul>
                <li>Items in stock</li>
                <li>A randomly generated recipe idea</li>
                <li>A list of upcoming events</li>
              </ul>
            </ul>
            <h3>Exploring Inventory</h3>
            <img src="../tutorial_recipient_image/inventory.png" alt="Inventory" />
            <ul>
              <li>Navigate to the Inventory section from the menu</li>
              <li>Browse available pantry items, including details like quantity and location</li>
              <li>Use the search bar to quickly find specific items</li>
            </ul>

            <h3>Submitting a Request</h3>
            <ul>
              <li>Go to the Request page</li>
              <img src="../tutorial_recipient_image/request.png" alt="Requset" />
              <li>Submit the items you need from the inventory list by clicking the "Request" button</li>
              <li>Enter the quantity and submit your request</li>
              <li>Check the status of your requests in real time</li>
              <li>You can edit or delete incomplete requests, but you cannot edit or delete completed ones</li>
            </ul>

            <h3>Viewing Events</h3>
            <img src="../tutorial_recipient_image/event.png" alt="Events" />
            <ul>
              <li>Visit the Event page to stay informed about pantry activities</li>
              <li>Check details for upcoming events like food drives, workshops, or distribution days</li>
              <li>Include past events to review information if needed</li>
              <li>Receive email notifications about newly added or updated events. You can turn email notifications off from the Profile section if desired</li>
            </ul>

            <h3>Recipe Idea</h3>
            <img src="../tutorial_recipient_image/recipeidea.png" alt="Recipe Idea" />
            <ul>
              <li>Access the Recipe Idea section for meal inspiration</li>
              <li>View recipes based on items currently in stock at the pantry</li>
              <li>Customize recipes by adding additional ingredients you already have</li>
              <li>Green: In stock</li>
              <li>Blue: Few items left</li>
              <li>Red: No stock available</li>
            </ul>

            <h3>Accessing the Pantry Map</h3>
            <img src="../tutorial_recipient_image/map.png" alt="Pantry Map" />
            <ul>
              <li>Open the Map page to visualize the pantry layout</li>
              <li>Use this feature to save time during your pantry visits</li>
            </ul>

            <h3>Contacting Pantry Staff</h3>
            <img src="../tutorial_recipient_image/contactpantry.png" alt="Contact Pantry" />
            <ul>
              <li>Navigate to the Contact Pantry page to communicate directly with staff</li>
              <li>Send inquiries about your requests, items, or general pantry information</li>
              <li>Use this feature for quick support or clarification</li>
            </ul>

            <h3>Help Section</h3>
            <img src="../tutorial_recipient_image/help.png" alt="Help Section" />
            <ul>
              <li>Visit the Help page for FAQs and troubleshooting</li>
              <li>Learn how to navigate the platform and make the most of its features</li>
              <li>Use the category buttons in the upper-left corner or the search bar for easier navigation</li>
            </ul>

          </div>
        </section>

       

        <section id="tutorial-recipient-tips">
          <h2>Tips for Maximizing Your Experience</h2>
          <div className="tutorial-section-content">
            <ul>
              <li>Stay Updated: Regularly check your dashboard for new items or announcements</li>
              <li>Plan Ahead: Submit requests early to ensure your items are available</li>
              <li>Use Notifications: Enable notifications to receive real-time updates about your requests and events</li>
              <li>Leverage Recipes: Use the Recipe Idea feature to make the most of the items you receive</li>
            </ul>
          </div>
        </section>

        <section id="tutorial-recipient-help">
          <h2>Need Help?</h2>
          <div className="tutorial-section-content">
            <p>If you encounter any issues or need assistance:</p>
            <ul>
              <li>Send an email to <a href="mailto:pantryhelper.24@gmail.com">pantryhelper.24@gmail.com</a></li>
              <li>Reach out to pantry staff through the Contact Pantry feature</li>
              <li>Check the Help section for answers to common questions</li>
            </ul>
          </div>
        </section>
      </div>
    </div>
  );
};

export default TutorialRecipient;
