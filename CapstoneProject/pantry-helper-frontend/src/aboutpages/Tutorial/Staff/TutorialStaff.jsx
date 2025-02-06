import React from 'react';
import { Link } from 'react-router-dom';
import './TutorialStaff.css';

const TutorialStaff = () => {
  return (
    <div id="tutorial-staff-container">
      <div id="tutorial-staff-header">
        <div id="tutorial-staff-nav-buttons">
         
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

      <div id="tutorial-staff-content">
        <h1 id="tutorial-staff-title">Staff Tutorial</h1>

        <section id="tutorial-staff-getting-started">
          <div className="tutorial-section-content">
            <h3>Login</h3>
            <img src="../tutorial_staff_image/stafflogin.png" alt="Staff Login" />
            <ul>
              <li>Use your credentials or Google login to access your staff account</li>
              <li>Register as a pantry staff by clicking on "Register as a pantry staff" in the profile section</li>
              <li>Select your pantry and enter the access code for your pantry</li>
              <li>If you do not know your pantry access code, send an email to <a href="mailto:pantryhelper.24@gmail.com">pantryhelper.24@gmail.com</a> to reset your access code</li>
            </ul>

            <h3>Requesting a Pantry</h3>
            <img src="../tutorial_staff_image/requestpantry.png" alt="Request Pantry" />
            <ul>
              <li>You can request a new pantry by clicking on "Request a new pantry" in the profile section</li>
              <li>Fill in your name, email, and pantry name. The admin will review your request and contact you within 5 business days</li>
            </ul>

            <h3>Dashboard Overview</h3>
            <img src="../tutorial_staff_image/dashboard.png" alt="Dashboard" />
            <ul>
              <li>Upon logging in, the Dashboard provides a quick summary of key pantry metrics:</li>
              <ul>
                <li>Daily incoming and outgoing items report</li>
                <li>A list of low-stock items</li>
                <li>Upcoming events</li>
              </ul>
            </ul>

            <h3>Managing Inventory</h3>
            <img src="../tutorial_staff_image/inventory.png" alt="Inventory" />
            <ul>
              <li>Navigate to the Inventory section to manage pantry stock</li>
              <li>Add Items: Enter item details such as name, quantity, and expiration date</li>
              <li>Batch Upload: Batch upload a list of incoming items using a .txt, .pdf, or .xls file</li>
              <li>Mark as Outgoing: Select an item, input the outgoing date and quantity, and mark it as outgoing</li>
              <li>Filter: Filter items by date, quantity, or stock status</li>
              <li>Search: Search for items by name or keyword</li>
            </ul>

            <h3>Financial Management</h3>
            <img src="../tutorial_staff_image/finance.png" alt="Finance" />
            <ul>
              <li>Navigate to the Finance section to track income and expenses</li>
              <li>Add or update records for donations, grants, and operational costs</li>
              <li>Review financial reports to aid in budget planning and maintain transparency</li>
            </ul>

            <h3>Tracking Donations</h3>
            <img src="../tutorial_staff_image/incomingoutgoing.png" alt="Incoming Outgoing" />
            <ul>
              <li>Go to the Reports - Incoming/Outgoing Items page</li>
              <li>Review detailed logs of donated and distributed items</li>
              <li>Filter data by item name or date for deeper insights. Multiple item selection is available</li>
              <li>Hover over graphs for more details</li>
            </ul>

            <h3>Popular Items</h3>
            <img src="../tutorial_staff_image/popular.png" alt="Popular Items" />
            <ul>
              <li>Identify high-demand products to better meet recipient needs</li>
              <li>Bar Chart: Displays the top 7 popular items</li>
              <li>Table: Shows item name, popularity, and stock status</li>
              <li>Search for multiple items and sort by clicking on the table header</li>
              <li>Green: Popularity 70% and above</li>
              <li>Blue: Popularity 20%–70%</li>
              <li>Red: Popularity below 20%</li>
            </ul>

            <h3>Expired Items</h3>
            <img src="../tutorial_staff_image/expired.png" alt="Expired Items" />
            <ul>
              <li>View lists of expired and soon-to-expire items for efficient stock rotation</li>
              <li>Search for items by name</li>
              <li>Delete items directly from this page</li>
              <li>For soon-to-expire items, the default is 30 days left, but you can input custom dates</li>
            </ul>

            <h3>Handling Recipient Requests</h3>
            <img src="../tutorial_staff_image/requests.png" alt="Requests" />
            <ul>
              <li>Visit the Report - Requested Items section to manage item requests</li>
              <li>View Requests: See all submitted requests categorized by status (Pending, Fulfilled)</li>
              <li>Mark a request as complete by clicking the green check button</li>
              <li>Include completed requests in the list or revert fulfilled requests by clicking the blue revert button</li>
              <li>Notify Recipients: Automatically send updates to recipients about the status of their requests</li>
              <li>Search and Filter: Search requests or filter them by date</li>
            </ul>

            <h3>Income & Expense Reports</h3>
            <img src="../tutorial_staff_image/financereport.png" alt="Income Reports" />
            <ul>
              <li>Analyze financial trends and create actionable plans</li>
              <li>Hover over graphs for more details</li>
              <li>Filter by date for specific reports</li>
            </ul>

            <h3>Managing Members</h3>
            <img src="../tutorial_staff_image/member.png" alt="Members" />
            <ul>
              <li>Go to the Member section to view and manage staff and recipient roles</li>
              <li>Remove inactive members</li>
              <li>Ensure permissions align with user responsibilities</li>
            </ul>

            <h3>Organizing and Promoting Events</h3>
            <img src="../tutorial_staff_image/event.png" alt="Events" />
            
            <ul>
              <li>Use the Events page to add, edit, or delete pantry events</li>
              <li>Enter event details, including date, location, description, and select an icon</li>
              <li>Green: Fundraising events</li>
              <li>Red: Special events (e.g., Thanksgiving)</li>
              <li>Blue: All other events</li>
              <li>Notify users with email and in-app notifications about newly added or updated events</li>
              <li>Include past events in the list by clicking on "Include past events"</li>
              <li>Edit or delete events as needed</li>
              <div className="event-image-container">
              <img src="../tutorial_staff_image/eventadd.png" alt="Event Add" />
            <p>Email sent to the recipient when a new event is added</p>
            </div>
            <div className="event-image-container">
            <img src="../tutorial_staff_image/eventcancel.png" alt="Event Cancel" />
            <p>Email sent to the recipient when an event is deleted</p>
            </div>

            </ul>

            <h3>Creating a Pantry Map</h3>
            <img src="../tutorial_staff_image/map.png" alt="Map" />
            <ul>
              <li>Use the Map feature to create a layout for your pantry</li>
              <li>Add boxes, name them, and associate stored items with each box</li>
              <li>Updates to the map will reflect in the inventory system</li>
              <li>Save the map after making changes</li>
            </ul>

            
          </div>
        </section>

        <section id="tutorial-staff-tips">
          <h2>Tips for Streamlining Operations</h2>
          <div className="tutorial-section-content">
            <ul>
              <li>Monitor Stock Levels: Regularly check inventory and restock essential items to avoid shortages</li>
              <li>Fulfill Requests Promptly: Address recipient requests quickly to ensure satisfaction</li>
              <li>Leverage Analytics: Use reports to identify trends and optimize pantry operations</li>
              <li>Promote Events: Notify users well in advance to increase participation</li>
            </ul>
          </div>
        </section>

        <section id="tutorial-staff-help">
          <h2>Need Help?</h2>
          <div className="tutorial-section-content">
            <p>If you encounter any issues or need assistance:</p>
            <ul>
              <li>Send an email to <a href="mailto:pantryhelper.24@gmail.com">pantryhelper.24@gmail.com</a></li>
              <li>Check the Help section for answers to common questions</li>
            </ul>
          </div>
        </section>
      </div>
    </div>
  );
};

export default TutorialStaff;
