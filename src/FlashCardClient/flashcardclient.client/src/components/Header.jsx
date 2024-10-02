import { useAuth } from "../context/AuthContext";
import Button from "react-bootstrap/Button";
import Container from "react-bootstrap/Container";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import NavDropdown from "react-bootstrap/NavDropdown";

function Header() {
  const { isAuthenticated, user, login, logout } = useAuth();

  return (
    <Navbar bg="primary" expand="sm" className="bg-body-tertiary">
      <Container>
        <Navbar.Brand href="#home">AI-Powered Flash Card</Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          {/* <Nav className="me-auto">
            <Nav.Link href="#home">Home</Nav.Link>
            <Nav.Link href="#link">Link</Nav.Link>
          </Nav> */}
          <Nav className="ms-auto">
            {!isAuthenticated && (
              <Button variant="outline-secondary" onClick={() => login()}>Log in</Button>
            )}
            {isAuthenticated && (
              <NavDropdown title={user.name} id="basic-nav-dropdown">
                <NavDropdown.Item onClick={() => logout()}>Log out</NavDropdown.Item>
              </NavDropdown>
            )}
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
}

export default Header;
