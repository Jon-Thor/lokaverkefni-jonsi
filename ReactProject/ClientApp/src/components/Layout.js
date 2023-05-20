import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';
import { UserProvider } from './UserContext';



export class Layout extends Component {
  static displayName = Layout.name;

  render() {
      return (
          <div>
              <UserProvider>
            <NavMenu />
            <Container style={{ padding: 0, } }>
                
          {this.props.children}
                  </Container>
              </UserProvider>
      </div>
    );
  }
}
