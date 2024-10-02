import { Form as RForm, Outlet } from "react-router-dom";
import Header from "../components/Header";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";

export default function Root() {
  return (
    <>
      <div className="d-flex flex-column min-vh-100 container-fluid">
        <Header />
        <Container className="h-100">
          <Row className="h-100">
            <Col
              sm={3}
              className="h-100 p-3 d-flex flex-column bg-body-secondary"
            >
              <RForm method="post">
                <div className="d-grid gap-2">
                  <Button
                    variant="outline-primary"
                    size="sm"
                    type="submit"
                    
                  >
                    New
                  </Button>
                </div>
              </RForm>
              <RForm className="mt-3">
                <Form.Group
                  className="mb-3"
                  controlId="exampleForm.ControlInput1"
                >
                  <Form.Control type="email" placeholder="Type to search" />
                </Form.Group>
              </RForm>
            </Col>
            <Col sm={9}>
              <Outlet />
            </Col>
          </Row>
        </Container>
      </div>
    </>
  );
}
