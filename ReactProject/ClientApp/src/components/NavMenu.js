import React, { Component } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { UserContext } from './UserContext';

export class NavMenu extends Component {
    static displayName = NavMenu.name;
    static contextType = UserContext; 

  constructor (props) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true
    };
  }

  toggleNavbar () {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

    render() {
        const { loggedinUser } = this.context;
        const { logout } = this.context;
      return (        
          <header style={{maxWidth: "1200px", marginInline: "auto", backgroundColor: "#1DA1F2", position: 'sticky', top: '0px', }} className="testHeader">
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow " container>
                <NavbarBrand tag={Link} to="/">Twitter</NavbarBrand>
                <NavbarToggler onClick={this.toggleNavbar} />
          <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
            <ul className="navbar-nav flex-grow nav">
              <NavItem>
                <NavLink tag={Link} to="/">Home</NavLink>
              </NavItem>
                          {loggedinUser ? (
                              <div className="login-style">
                              <NavItem>
                                  <NavLink tag={Link} to={`/profile/${this.context.loggedinUser.userId}`}>Profile</NavLink>
                              </NavItem>
                                  <NavItem>
                                      <NavLink className="navLink" onClick={logout}>logout</NavLink>
                                  </NavItem>
                              </div>
                          ) : (
                              <NavItem>
                                  <NavLink tag={Link} to="CreateAccount">Login/Create</NavLink>
                              </NavItem>
                          )}

            </ul>
          </Collapse>
        </Navbar>
      </header>
    );
  }
}
