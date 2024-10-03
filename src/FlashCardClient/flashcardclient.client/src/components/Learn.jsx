import Carousel from "react-bootstrap/Carousel";
import FlashCard from "./FlashCard";

export default function Learn({ cards }) {
  return (
    <Carousel className="bg-secondary bg-opacity-75 w-100 h-100" interval={null}>
      {cards.map((card) => (
        <Carousel.Item key={card.id} className="" >
          <div className="d-flex w-100 justify-content-center" style={{marginTop: "10rem", height: 500}}>
          <FlashCard
            id={card.id}
            word={card.word}
            meaning={card.meaning}
            sentenses={["I like apple"]}
          />
          </div>
          <Carousel.Caption></Carousel.Caption>
        </Carousel.Item>
      ))}
    </Carousel>
  );
}
