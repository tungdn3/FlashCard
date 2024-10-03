import {
  Form as RForm,
  Outlet,
  useLoaderData,
  NavLink,
  json,
} from "react-router-dom";
import { useState } from "react";
import Header from "../components/Header";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import Button from "react-bootstrap/Button";
import ListGroup from "react-bootstrap/ListGroup";
import Modal from "react-bootstrap/Modal";
import Form from "react-bootstrap/Form";
import axios from "axios";

export async function loader({ request }) {
  const pageSize = 1000; // todo: handle paging properly
  const url = new URL(request.url);
  const searchText = url.searchParams.get("searchText");
  const pageNumber = parseInt(url.searchParams.get("page")) || 1;
  let apiUrl = `/v1/decks?pageSize=${pageSize}&pageNumber=${pageNumber}`;
  if (searchText) {
    apiUrl += `&searchText=${searchText}`;
  }
  const response = await fetch(apiUrl);
  if (response.status === 401) {
    window.location.href = "/auth/login";
  }
  const pageResult = await response.json();
  return { decks: pageResult.items, searchText, pageNumber };
}

export async function action({ request }) {
  const formData = await request.formData();
  const intent = formData.get("intent");
  if (intent === "create") {
    const deckName = formData.get("name");
    await axios.post(`/v1/decks`, { name: deckName });
    return { ok: true };
  }

  if (intent === "delete") {
    const deckId = formData.get("id");
    await axios.delete(`/v1/decks/${deckId}`);
    return { ok: true };
  }

  throw json({ message: "Invalid intent" }, { status: 400 });
}

export default function Root() {
  const { decks } = useLoaderData();

  const [showCreateDeckModal, setShowCreateDeckModal] = useState(false);

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
              <div className="border-bottom">
                <div className="d-grid gap-2">
                  <Button
                    variant="secondary"
                    size="sm"
                    type="submit"
                    onClick={() => setShowCreateDeckModal(true)}
                  >
                    New Deck
                  </Button>
                </div>
                <RForm className="mt-3">
                  <Form.Group
                    className="mb-3"
                    controlId="exampleForm.ControlInput1"
                  >
                    <Form.Control
                      className="py-2"
                      placeholder="Type to search"
                    />
                  </Form.Group>
                </RForm>
              </div>

              <nav className="mt-4">
                {decks.length ? (
                  <ListGroup defaultActiveKey="3">
                    {decks.map((deck) => (
                      <div
                        key={deck.id}
                        className="d-flex justify-content-between align-items-center"
                      >
                        <NavLink
                          key={deck.id}
                          to={`decks/${deck.id}`}
                          className={({ isActive, isPending }) =>
                            isActive
                              ? "bg-primary text-light p-2"
                              : isPending
                              ? "bg-secondary text-light p-2"
                              : "p-2"
                          }
                        >
                          {deck.name}
                        </NavLink>
                        <RForm method="post">
                          <Form.Control
                            hidden
                            name="id"
                            defaultValue={deck.id}
                          />
                            <Button
                              type="submit"
                              size="sm"
                              name="intent"
                              defaultValue="delete"
                              variant="outline-danger"
                            >
                              <i className="bi bi-trash-fill"></i>
                            </Button>
                        </RForm>
                      </div>
                    ))}
                  </ListGroup>
                ) : (
                  <p>
                    <i>No decks</i>
                  </p>
                )}
              </nav>
            </Col>
            <Col sm={9}>
              <Outlet />
            </Col>
          </Row>
        </Container>
      </div>

      <Modal
        show={showCreateDeckModal}
        onHide={() => setShowCreateDeckModal(false)}
      >
        <Modal.Header closeButton>
          <Modal.Title>Create Deck</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <RForm method="post">
            <Form.Group className="mb-3">
              <Form.Control name="name" placeholder="Deck name" autoFocus />
            </Form.Group>
            <div className="d-flex">
              <Button
                variant="primary"
                type="submit"
                name="intent"
                defaultValue="create"
                className="ms-auto"
                onClick={() => setShowCreateDeckModal(false)}
              >
                Create
              </Button>
            </div>
          </RForm>
        </Modal.Body>
      </Modal>
    </>
  );
}
